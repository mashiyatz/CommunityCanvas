<style lang="scss" scoped>
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

    #sidebar {
        color: var(--white-color);
        font-family: monospace;
        z-index: 1;
        position: relative;
        top: -3rem;
        left: 0;
        display: none;
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

        #sidebar {
            display: block;
        }

        .menu-title {
            display: block;
            text-align: left;
            padding-bottom: 1rem;
            font-size: 32px;
        }

        .menu-content {
            display: flex;
            flex-direction: column;
            width: 100%;
            overflow: scroll;
            
            .model-menu {
                display: grid;
                grid-template-columns: 1fr 1fr;
                grid-template-rows: auto;
                column-gap: 1rem;
                row-gap: 1rem;

                .model-image-button {
                outline: none;
                    img {
                        width: 100%;
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
import { storeToRefs } from 'pinia';
import { ref } from 'vue';


const storeLoc = locationStore();
const storeURLs = urlStore(); 
const { lat, lng, bearing, pitch, zoom } = storeToRefs(storeLoc);
const { urls } = storeToRefs(storeURLs);
const isExpanded = ref(false);

const ToggleMenu = () => {
    isExpanded.value = !isExpanded.value;
    // urls.value.forEach( (item) => console.log(item));
}

</script>

<template v-model="store">
    <aside :class="`${isExpanded ? 'isExpanded' : ''}`">
        <div class="logo">
            <img src="../assets/icons/LOGO.png" alt="Community Canvas logo">
        </div>
        <div class="menu-toggle-wrap">
            <button class="menu-toggle" @click="ToggleMenu">
                <span class="material-symbols-outlined">keyboard_double_arrow_right</span>
            </button>
        </div>
        <div id="sidebar">
            Longitude: {{ lng.toFixed(4) }} <br>
            Latitude: {{ lat.toFixed(4) }} <br>
            Zoom: {{ zoom.toFixed(2) }} <br>
            <template v-if="bearing"> Bearing: {{ bearing.toFixed(2) }} <br> </template>
            <template v-if="pitch"> Pitch: {{ pitch.toFixed(2) }} <br> </template>
            <button
                @click="storeLoc.$patch({ lng: -73.8892669226548, lat: 40.753826358076516, bearing: 0, pitch: 60, zoom: 20 })">Reset
                Camera</button>
        </div>
        <h3 class="menu-title">Models</h3>
        <div class="menu-content">
            <div class="model-menu">
                <button class="model-image-button" v-for="(item, index) in urls" @click="storeURLs.RetrieveModelURL(index)">
                    <img :src="item" alt="no image">
                </button>                
            </div>
        </div>
        

    </aside>
</template>