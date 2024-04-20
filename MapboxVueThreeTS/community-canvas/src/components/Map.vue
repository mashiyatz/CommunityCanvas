<template>
  <div id="mapContainer" ref="mapContainer" class="map-container"></div>
</template>

<script lang="ts">
// import * as mapboxgl from "mapbox-gl";
import {Map as mbMap, type AnyLayer} from "mapbox-gl";
import { mapboxAccessToken } from '../global';
import { Threebox } from 'threebox-plugin';
import { locationStore } from "@/stores/location";
import { urlStore } from "@/stores/s3urls";
import { storeToRefs } from 'pinia';

const mapboxgl = require('mapbox-gl');
mapboxgl.accessToken = mapboxAccessToken;

declare global {
  interface Window {
    tb: any;
  }
}

interface mapBoxMap {
    map: mbMap | null;
}

class MapboxModelLayerCreator {

  // private renderer: THREE.WebGLRenderer = new THREE.WebGLRenderer;
  private createCustomLayer(layerName: string, origin: [number, number]) {

    // let model;
    //create the layer
    let customLayer3D: AnyLayer = {
      id: layerName,
      type: 'custom',
      renderingMode: '3d',
      onAdd: function (map: mbMap, gl: WebGLRenderingContext) {
        window.tb = new Threebox(
          map,
          map.getCanvas().getContext('webgl'),
          {
            // realSunlight: true,
            enableSelectingObjects: true,
            enableDraggingObjects: true,
            enableRotatingObjects: true,
            enableTooltips: false,
          }
        )
      },
      render: function (gl: WebGLRenderingContext, matrix: number[]) {
        window.tb.update();
      }
    };
    return customLayer3D;
  };

  constructor(map: mbMap) {

    let modelOrigin: [number, number] = [-73.8892669226548, 40.753826358076516];
    let modelLayer: AnyLayer;

    map.on('style.load', () => {
      map.setConfigProperty('basemap', 'lightPreset', 'dusk');
      modelLayer = this.createCustomLayer('3d-model', modelOrigin);
      map.addLayer(modelLayer);
      window.tb.defaultLights();

      const storeURLs = urlStore();
      const { modelURL } = storeToRefs(storeURLs);

      map.on('click', (e) => {
        if (modelURL.value === null) return;
        let options = {
          type: 'gltf', //'gltf'/'mtl'
          obj: modelURL.value,
          units: 'meters', //units in the default values are always in meters
          scale: 0.3,
          rotation: { x: 90, y: 0, z: 0 }, //default rotation
          anchor: 'center'
        }
        window.tb.loadObj(options, (model: any) => {
          model.setCoords([e.lngLat.lng, e.lngLat.lat]);
          // model.addTooltip("A tree in a park", false);
          window.tb.add(model, modelLayer.id);
          // model.castShadow = true;
          window.tb.lights.dirLight.target = model;
          window.tb.update();
        });
        storeURLs.$patch({modelURL: null});
      });

    });
  }
}

export default {

  data: () => ({ map: null } as mapBoxMap),

  mounted() {
    const store = locationStore();
    const map = new mbMap({
      container: "mapContainer",
      style: 'mapbox://styles/mapbox/standard',
      zoom: 20,
      center: [-73.8892669226548, 40.753826358076516],
      pitch: 60,
      antialias: true // create the gl context with MSAA antialiasing, so custom layers are antialiased
    });

    const updateLocation = () => {
      store.$patch(this["getLocation"]());
    }

    map.on("move", updateLocation);
    map.on("zoom", updateLocation);
    map.on("rotate", updateLocation);
    map.on("pitch", updateLocation);

    store.$subscribe((mutation, state) => {
      const curr = this["getLocation"]();
      const map = this["map"];

      if (curr["lng"] != state.lng || curr["lat"] != state.lat) {
        map.setCenter({ lng: state.lng, lat: state.lat });
      }
      if (curr["pitch"] != state.pitch) { 
        map.setPitch(state.pitch);
      }
      if (curr["bearing"] != state.bearing) { 
        map.setBearing(state.bearing);
      }
      if (curr["zoom"] != state.zoom) { 
        map.setZoom(state.zoom);
      }
    })

    this["map"] = map;
    const modelLayer = new MapboxModelLayerCreator(this["map"]);

  },

  unmounted() {
    if (this["map"] != null) { 
      this["map"].remove();
      this["map"] = null;
    }
  },
  methods: {
    getLocation() {
      return {
        ...this["map"].getCenter(),
        bearing: this["map"].getBearing(),
        pitch: this["map"].getPitch(),
        zoom: this["map"].getZoom(),
      };
    },
  },
};
</script>

<style>
.map-container {
  flex: 1;
}
</style>