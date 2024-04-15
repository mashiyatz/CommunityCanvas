import { defineStore } from 'pinia';
import { storeToRefs } from 'pinia';
import { S3Client, ListObjectsV2Command } from "@aws-sdk/client-s3";
import AWS from "aws-sdk";

interface S3List {
    urls: Array<string>;
    modelURL: string;
}

export const urlStore = defineStore('urlList', {
    state: (): S3List => {
        return {
            urls: new Array<string>(),
            modelURL: null
        }
    },
    actions: {
        RetrieveModelURL(index: number) {
            let imgURL: string = this.urls[index];
            if (imgURL.split("Thumbnails/")[1].split("/")[0] === "Trees") {
                this.modelURL = imgURL.split("Thumbnails/").join("").split(".png").join("").concat(".glb");
            } else {
                this.modelURL = imgURL.split("Thumbnails/").join("").split(".png").join("").concat(".gltf");
            }            
        }
    }
})

export const searchBuckets = async () => {
    const command = new ListObjectsV2Command({
        Bucket: "community-canvas-bucket",
        Prefix: "CCModels/Thumbnails/",
        MaxKeys: 100,
    });

    try {
        let isTruncated = true;

        console.log("Your bucket contains the following objects:\n");
        let contents = "";
        const prefix = "https://community-canvas-bucket.s3.amazonaws.com/";
        const store = urlStore();
        const { urls } = storeToRefs(store);

        while (isTruncated) {
            const { Contents, IsTruncated, NextContinuationToken } = await client.send(command);
            Contents.map((c) => {
                store.$patch({ urls: urls.value.concat(prefix + c.Key) });
                contents += `${prefix + c.Key}\n`;
            }
        );
            isTruncated = IsTruncated;
            command.input.ContinuationToken = NextContinuationToken;
        }
        // console.log(contents);
    } catch (err) {
        console.error(err);
    }
};

const cognitoCredentials = new AWS.CognitoIdentityCredentials(
    {
        IdentityPoolId: 'us-east-1:5d854e3e-4399-4452-91b0-178ee7f8c110',
    },
    {
        region: 'us-east-1',
        httpOptions: { timeout: 1000 }
    }
);

const client = new S3Client({
    region: "us-east-1",
    credentials: cognitoCredentials,
});

cognitoCredentials.get((err) => {
    if (err) {
        console.error('Error getting credentials:', err);
    } else {
        console.log("Success!");
        AWS.config.credentials = cognitoCredentials;
        console.log(AWS.config.credentials);
        searchBuckets();
    }
});
