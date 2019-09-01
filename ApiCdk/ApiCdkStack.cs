using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;

namespace ApiCdk
{
    public class ApiCdkStack : Stack
    {
        public ApiCdkStack(Construct parent, string id, IStackProps props) : base(parent, id, props)
        {
            var bkt = Bucket.FromBucketName(parent, "tyst-buck", "tyst-buck-xx");
            if (bkt == null)
                new Bucket(this, "tyst-buck", new BucketProps
                {
                    Versioned = false,
                    BucketName = "tyst-buck-xx"
                });

            var api = new RestApi(this, "test-api123-xx", new RestApiProps
            {
                RestApiName = "Test API 123",
                DeployOptions = new StageOptions
                {
                    StageName = "v1"
                }
            });

            api.Root.AddMethod("ANY");

            var apiSam = api.Root.AddResource("apisam");

            var apiSamLambda = new Function(this, "apisam-lambda", new FunctionProps {
                Code = Code.FromAsset("../ApiSam/bin/Publish"),
                Handler = "ApiSam::ApiSam.LambdaEntryPoint::FunctionHandlerAsync",
                Runtime = Runtime.DOTNET_CORE_2_1,
                Role = Role.FromRoleArn(this, "lambda-basic", "arn:aws:iam::324054559184:role/lambda-basic")
            });

            var apiSamProxy = apiSam.AddProxy(new ResourceOptions
            {
                DefaultIntegration = new LambdaIntegration(apiSamLambda),
            });
        }
    }
}
