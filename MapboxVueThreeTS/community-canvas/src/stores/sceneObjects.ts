import { defineStore } from 'pinia';
import { storeToRefs } from 'pinia';

interface ModelArray {
    modelsInScene: Array<ModelDetails>,
}

interface ModelDetails {
    modelURL: string;
    modellng: number, 
    modellat: number,
}

export const sceneStore = defineStore('sceneObjects', {
    state: (): ModelArray => {
        return {
            modelsInScene: new Array<ModelDetails>(),
        }
    },
    actions: {
        AddToModelArray(url: string, lng: number, lat: number) {
            let newModelDetails: ModelDetails = {
                modelURL: url, 
                modellng: lng, 
                modellat: lat,
            };
            this.modelsInScene.push(newModelDetails);
        }
    }

})