import { ListBucketsCommand, S3Client } from "@aws-sdk/client-s3";
import AWS, { S3 } from "aws-sdk";

// var credentials = new SharedIniFileCredentials({profile: 'default'});
// AWS.config.credentials = credentials;

export class TestingS3 {

    private cognitoCredentials = new AWS.CognitoIdentityCredentials(
        {
            IdentityPoolId: 'us-east-1:5d854e3e-4399-4452-91b0-178ee7f8c110',
        }, 
        {
            region: 'us-east-1',
            httpOptions: { timeout: 1000 }
        });

    private async listBuckets(client: S3Client, command: ListBucketsCommand) {
        try {
            const { Owner, Buckets } = await client.send(command);
            if ( Buckets === undefined ) return;
            if ( Owner === undefined) return;
            console.log(
              `${Owner.DisplayName} owns ${Buckets.length} bucket${
                Buckets.length === 1 ? "" : "s"
              }:`,
            );
            console.log(`${Buckets.map((b) => ` • ${b.Name}`).join("\n")}`);
          } catch (err) {
            console.error(err);
          }
    }

    constructor() {
        this.cognitoCredentials.get((err) => {
            if (err) {
                console.error('Error getting credentials:', err);
            } else {
                console.log("Success!");
                AWS.config.credentials = this.cognitoCredentials;
                console.log(AWS.config.credentials);
                const client = new S3Client({
                    region: "us-east-1",
                    credentials: this.cognitoCredentials
                });
                const command = new ListBucketsCommand({});
                this.listBuckets(client, command);
            }
        });
    }
}


