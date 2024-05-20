<style lang="scss" scoped>

#return-to-home {
    z-index: 99;
    display: block;
    position: absolute;
    right: 1rem;
    top: 1rem;
    background: none;
    border: none;
    .material-symbols-outlined {
        transition: 0.2s ease-out;
        font-size: 4rem;
        color: var(--white-color);
    }
}

#zoom-in {
    z-index: 99;
    display: block;
    position: absolute;
    right: 1rem;
    top: 6rem;
    background: none;
    border: none;
    .material-symbols-outlined {
        transition: 0.2s ease-out;
        font-size: 4rem;
        color: var(--white-color);
    }
}

#zoom-out {
    z-index: 99;
    display: block;
    position: absolute;
    right: 1rem;
    top: 11rem;
    background: none;
    border: none;
    .material-symbols-outlined {
        transition: 0.2s ease-out;
        font-size: 4rem;
        color: var(--white-color);
    }
}

#infobox {

    h3 {
        display: none;
    }

    ol {
        display: none;
    }

    &.isInfoOpen{
        transition: 0.2s ease-in-out;
        display: flex;
        flex-direction: column;
        position: absolute;
        background-color: var(--default-color);
        opacity: 90%;
        width: 48vw;
        height: 50vh;
        z-index: 99;
        right: 0;
        margin: 1rem;
        border-radius: 20px;
        overflow: scroll;

        h3 {
        display: block;
        margin: 1.5rem;
        // font-size: 2rem;
        color: var(--white-color);
        }

        ol {
            // margin-left: 1rem;
            padding-right: 1rem;
            display: block;
            // font-size: 1.5rem;
            color: var(--white-color);
        }
    }
}

#position-ui {
    display: none;

    &.isSelected {
        display: block;
        position: absolute;
        background: none;
        z-index: 99;
        bottom: 2rem;
        right: 2rem;

        #d-pad {
            display: grid;
            grid-template-columns: 1fr 1fr 1fr;
            grid-template-rows: auto;
            column-gap: 1rem;
            row-gap: 1rem;
            

            button {
                border: none;
                background: none;

                .material-symbols-outlined {
                    text-align: center;
                    transition: 0.2s ease-out;
                    font-size: 2rem;
                    color: var(--white-color);
                }
            }

            #up {
                grid-row: 1;
                grid-column: 2;
            }

            #down {
                grid-row: 2;
                grid-column: 2;
            }

            #right {
                grid-row: 2;
                grid-column: 3;
            }

            #left {
                grid-row: 2;
                grid-column: 1;
            }

            #clockwise {
                grid-row: 1;
                grid-column: 3;
            }

            #counter-clockwise {
                grid-row: 1;
                grid-column: 1;
            }
        }

    }
}

.info-toggle {
    position: absolute;
    right: 0;
    margin: 2rem;
    z-index: 99;
    border: none;
    background: none;

    .material-symbols-outlined {
                transition: 0.2s ease-out;
                font-size: 4rem;
                color: var(--white-color);
            }
}

aside {
    display: flex;
    position: absolute;
    flex-direction: column;
    overflow: hidden;
    height: 100vh;
    max-height: 100vh;
    padding: 1rem;
    background-color: var(--default-color);
    color: var(--white-color);
    width: calc(2rem + 32px);
    transition: 0.2s ease-out;
    z-index: 99;

    .logo {
        margin-bottom: 1rem;

        img {
            width: 2rem;
        }
    }

    .menu-toggle-wrap {
        display: flex;
        justify-content: flex-end;
        margin-bottom: 1rem;
        position: relative;
        top: 0;
        transition: 0.2s ease-out;

        .menu-toggle {
            transition: 0.2s ease-out;
            border: none;
            background: none;

            .material-symbols-outlined {
                transition: 0.2s ease-out;
                font-size: 2rem;
                color: var(--white-color);
            }

            &:hover {
                .material-symbols-outlined {
                    color: var(--default-color);
                    transform: translateX(0.5rem);
                }
            }
        }
    }

    .menu-title {
        display: none;
    }

    .menu-content {
        display: none;
    }

    &.isExpanded {
        width: var(--sidebar-width);

        .menu-toggle-wrap {
            top: -4rem;

            .menu-toggle {
                transform: rotate(-180deg);
            }
        }

        #menu-tabs {
            display: flex;
            flex-direction: row;

            button {
                display: block;
                background: none;
                border: none;
                text-align: left;
                margin-bottom: 1rem;
                opacity: 25%;

                h3 {
                    font-size: 32px;
                    color: var(--white-color);
                }

                &.isLibrary {
                    opacity: 100%;
                }

                &.isScene {
                    opacity: 100%;
                }
            }
        }



        .menu-content {
            display: flex;
            flex-direction: column;
            width: 100%;
            top: -4rem;
            height: 100%;
            
            
            .model-menu {
                overflow: scroll;
                display: grid;
                height: 100%;
                margin-bottom: 3rem;
                grid-template-columns: 1fr 1fr;
                grid-template-rows: auto;
                column-gap: 1rem;
                row-gap: 1rem;

                .model-image-button {
                    outline: none;
                    max-height: 125px;
                    img {
                        width: 100%;
                    }
                }

                .model-image-button:focus {
                    img {
                        opacity: 25%;
                    }
                }
            }
        }
    }
}
</style>

<script setup lang="ts">
import { locationStore } from '@/stores/location';
import { urlStore } from '@/stores/s3urls'; 
import { selectionStore } from '@/stores/selectedObject';
import { sceneStore } from '@/stores/sceneObjects';
import { storeToRefs } from 'pinia';
import { ref } from 'vue';


const storeLoc = locationStore();
const storeURLs = urlStore(); 
const storeSelect = selectionStore();
const storeScene = sceneStore();
const { lat, lng, bearing, pitch, zoom } = storeToRefs(storeLoc);
const { urls } = storeToRefs(storeURLs);
const { isSelected }  = storeToRefs(storeSelect);
const { modelsInScene } = storeToRefs(storeScene);
const isExpanded = ref(false);
const isInfoOpen = ref(true);
const isLibrary = ref(true);
const isScene = ref(false);

const ToggleMenu = () => {
    isExpanded.value = !isExpanded.value;
}

const ToggleInfo = () => {
    isInfoOpen.value = !isInfoOpen.value;
}

const TurnLibraryViewOn = () => {
    isLibrary.value = true;
    isScene.value = false;
}

const TurnSceneViewOn = () => {
    isLibrary.value = false;
    isScene.value = true;
}

const DeselectObject = () => {
    if (window.tb.map.selectedObject) window.tb.map.unselectObject();
}

const RotateObjectCounterClockwise = () => {
    if (window.tb.map.selectedObject) {
        window.tb.map.draggedObject = window.tb.map.selectedObject;
        console.log(window.tb.map.draggedObject.rotation);
        window.tb.map.draggedObject.setRotation({
            z: (window.tb.utils.degreeify((window.tb.map.draggedObject.rotation))[2] + 10) % 360,
        });
        console.log(window.tb.map.draggedObject.rotation);
    }
}

const RotateObjectClockwise = (e) => {
    if (window.tb.map.selectedObject) {
        window.tb.map.draggedObject = window.tb.map.selectedObject;
        console.log(window.tb.map.draggedObject.rotation);
        window.tb.map.draggedObject.setRotation({
            z: (window.tb.utils.degreeify((window.tb.map.draggedObject.rotation))[2] - 10) % 360,
        });
        console.log(window.tb.map.draggedObject.rotation);
    }
}

const ShiftObject = (direction) => {
    if (window.tb.map.selectedObject) {
        window.tb.map.draggedObject = window.tb.map.selectedObject;
        let options = [
            Number(window.tb.map.draggedObject.coordinates[0] + direction[0] * 0.00001), 
            Number(window.tb.map.draggedObject.coordinates[1] + direction[1] * 0.00001), 
            window.tb.map.draggedObject.modelHeight
        ];
        window.tb.map.draggedObject.setCoords(options);
        // CAREFUL! Shifting object doesn't change coords saved in sceneStore.
    }
}

const FlyToLoc = (lng: number, lat: number) => {
    window.tb.map.flyTo(
        {
            center: [lng, lat],
            essential: true,
        }
    )
}

const ZoomMap = (doZoomIn: boolean) => {

    if (doZoomIn) window.tb.map.setZoom(window.tb.map.getZoom() + 1);
    else window.tb.map.setZoom(window.tb.map.getZoom() - 1);
}

</script>

<template v-model="store">
    <aside :class="`${isExpanded ? 'isExpanded' : ''}`" @click="DeselectObject">
        <div class="logo">
            <img src="../assets/icons/LOGO.png" alt="Community Canvas logo">
        </div>
        <div class="menu-toggle-wrap">
            <button class="menu-toggle" @click="ToggleMenu">
                <span class="material-symbols-outlined">keyboard_double_arrow_right</span>
            </button>
        </div>
        <div class="menu-content">
            <div id="menu-tabs"> 
                <button :class="`${isLibrary ? 'isLibrary': ''}`" id="library-tab" @click="TurnLibraryViewOn"><h3>Library</h3></button>
                <h3 style="font-size: 32px; top: -0.5rem;">/</h3>
                <button :class="`${isScene ? 'isScene': ''}`" id="scene-tab" @click="TurnSceneViewOn"><h3>Scene</h3></button>
            </div>
            <div class="model-menu">
                <button class="model-image-button" v-if="isLibrary" v-for="(item, index) in urls" @click="storeURLs.RetrieveModelURL(index)">
                    <img :src="item" alt="no image">
                </button>   
                <button class="model-image-button" v-if="isScene" v-for="item in storeScene.modelsInScene" @click="FlyToLoc(item.modellng, item.modellat)"> 
                    <img :src="item.modelURL" alt="no image">
                </button>             
            </div>
        </div>
    </aside>

    <button id="return-to-home" @click="storeLoc.$patch({ lng: -73.8892669226548, lat: 40.753826358076516, bearing: 0, pitch: 60, zoom: 20 })">
        <span class="material-symbols-outlined">home</span>
    </button>

    <button id="zoom-in" @click="ZoomMap(true)">
        <span class="material-symbols-outlined">zoom_in</span>
    </button>

    <button id="zoom-out" @click="ZoomMap(false)">
        <span class="material-symbols-outlined">zoom_out</span>
    </button>

    <!-- <div id="infobox" :class="`${isInfoOpen ? 'isInfoOpen' : ''}`" @click="DeselectObject">
        <h3>Welcome to Community Canvas!</h3>
        <ol>
            <li>Drag around the map to explore the neighborhood!</li>
            <li>Choose an object from the menu on the left.</li>
            <li>Place the object anywhere on the map!</li>
            <li>If you'd like to share, take a screenshot and send it to mmz9260@nyu.edu</li>
        </ol>
    </div> -->
    <div id="position-ui" :class="`${isSelected ? 'isSelected' : ''}`">
        <div id="d-pad">
            <button id="up" @click="ShiftObject([0, 1])"><span class="material-symbols-outlined">arrow_circle_up</span></button>
            <button id="down" @click="ShiftObject([0, -1])"><span class="material-symbols-outlined">arrow_circle_down</span></button>
            <button id="left" @click="ShiftObject([-1, 0])"><span class="material-symbols-outlined">arrow_circle_left</span></button>
            <button id="right" @click="ShiftObject([1, 0])"><span class="material-symbols-outlined">arrow_circle_right</span></button>
            <button id="clockwise" @click="RotateObjectClockwise"><span class="material-symbols-outlined">rotate_right</span></button>
            <button id="counter-clockwise" @click="RotateObjectCounterClockwise"><span class="material-symbols-outlined">rotate_left</span></button>
        </div>
    </div>

    <!-- <button class="info-toggle" @click="ToggleInfo">
        <span class="material-symbols-outlined">help</span>
    </button> -->
</template>