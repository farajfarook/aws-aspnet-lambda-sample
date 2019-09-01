using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiCdk
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new App(null);
            new ApiCdkStack(app, "ApiCdkStack", new StackProps());
            app.Synth();
        }
    }
}
