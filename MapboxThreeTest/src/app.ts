import mapboxgl, { AnyLayer } from "mapbox-gl";
import * as THREE from "three";
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
import { mapboxAccessToken } from '../src/global'
import { Threebox } from 'threebox-plugin';
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { TestingS3 } from "./helloS3";

declare global {
    interface Window {
        tb: any;
    }
}

mapboxgl.accessToken = mapboxAccessToken;

let idToURL: Map<string, string> = new Map<string, string>([
    ["b1", "https://community-canvas-bucket.s3.amazonaws.com/CCModels/Park/bench_2.gltf"],
    ["b2", "https://community-canvas-bucket.s3.amazonaws.com/CCModels/Park/bench_2.gltf"],
    ["b3", "https://community-canvas-bucket.s3.amazonaws.com/CCModels/Park/bench_2.gltf"],
    ["b4", "https://community-canvas-bucket.s3.amazonaws.com/CCModels/Park/bench_2.gltf"]
]
);

let styles = {
    day: 'satellite-streets-v9',
    night: 'dark-v10'
}
let selectedStyle = styles.day;

var map = new mapboxgl.Map({
    container: 'map',
    style: 'mapbox://styles/mapbox/standard',
    zoom: 20,
    center: [-73.8892669226548, 40.753826358076516],
    pitch: 60,
    antialias: true // create the gl context with MSAA antialiasing, so custom layers are antialiased
});

let stats: Stats;

class AppThreebox {

    private objURL: string = "";

    private setUpButtons(): void {
        const buttonRoot = document.getElementById("item-container");
        if (!buttonRoot) return;
    
        const childElements = Object.values(buttonRoot.childNodes) as HTMLElement[];
        // console.log(childElements);
        for (const child of childElements) {
            child.addEventListener('click', () => {
                this.objURL = idToURL.get(child.id) ?? "https://vazxmixjsiawhamofees.supabase.co/storage/v1/object/public/models/tree-beech/model.gltf";
                console.log(`This url is ${this.objURL}`);
            }
            );
        }
    }

    private setDebugKeys(): void {
        document.addEventListener("keydown", (event: KeyboardEvent) => {

            console.log(`Key Pressed: ${event.key}`);
            switch (event.key) {
                case 'q': {
                    this.objURL = "https://community-canvas-bucket.s3.amazonaws.com/CCModels/Park/bench.gltf"; 
                    break;
                }
                case 'w': {
                    this.objURL = "https://community-canvas-bucket.s3.amazonaws.com/CCModels/Park/light.gltf";
                    break;
                }
                case 'e': {
                    this.objURL = "https://community-canvas-bucket.s3.amazonaws.com/CCModels/Park/fountain.gltf";
                    break;
                }
                case 'r': {
                    this.objURL = "https://community-canvas-bucket.s3.amazonaws.com/CCModels/Traffic/sign_stop.gltf";
                    break;
                }
                default: {
                    break;
                }

            }


        });
    }

    // private renderer: THREE.WebGLRenderer = new THREE.WebGLRenderer;
    private createCustomLayer(layerName: string, origin: [number, number]) {

        // let model;
        //create the layer
        let customLayer3D: mapboxgl.AnyLayer = {
            id: layerName,
            type: 'custom',
            renderingMode: '3d',
            onAdd: function (map: mapboxgl.Map, gl: WebGLRenderingContext) {
                window.tb = new Threebox(
                    map,
                    map.getCanvas().getContext('webgl'),
                    {
                        realSunlight: true,
                        sky: true,
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
        console.log("created new layer");
        return customLayer3D;
    };

    constructor() {

        let modelOrigin: [number, number] = [-73.8892669226548, 40.753826358076516];
        let modelLayer: AnyLayer;

        this.setUpButtons();
        this.setDebugKeys();

        map.on('style.load', () => {
            map.setConfigProperty('basemap', 'lightPreset', 'dusk');
            stats = new Stats();
            map.getContainer().appendChild(stats.dom);
            modelLayer = this.createCustomLayer('3d-model', modelOrigin);
            map.addLayer(modelLayer);
            window.tb.defaultLights();

            map.on('click', (e) => {
                if (this.objURL === "") return;
                let options = {
                    type: 'gltf', //'gltf'/'mtl'
                    obj: this.objURL,
                    units: 'meters', //units in the default values are always in meters
                    scale: 0.3,
                    rotation: { x: 90, y: 0, z: 0 }, //default rotation
                    anchor: 'center'
                }
                window.tb.loadObj(options, (model: any) => {
                    model.setCoords([e.lngLat.lng, e.lngLat.lat]);
                    // model.addTooltip("A tree in a park", false);
                    window.tb.add(model, modelLayer.id);
                    model.castShadow = true;
                    window.tb.lights.dirLight.target = model;
                    window.tb.update();
                });
                this.objURL = "";
            });
            
        });
    }
}


class App {

    private map: mapboxgl.Map;

    private camera: THREE.Camera = new THREE.Camera();
    private scene: THREE.Scene = new THREE.Scene();
    private renderer: THREE.WebGLRenderer = new THREE.WebGLRenderer();

    private id: number = 0;

    private createMap() {
        let bounds: [number, number, number, number] = [-73.96531962759687, 40.71650398136544, -73.85090773503727, 40.77187154866687];
        let newMap: mapboxgl.Map;

        newMap = new mapboxgl.Map({
            container: 'map',
            style: 'mapbox://styles/mapbox/standard',
            center: [-73.8892669226548, 40.753826358076516],
            zoom: 20,
            maxBounds: bounds,
            antialias: true
        });

        return newMap;
    }

    private createObjectOnCustomLayer(
        modelOrigin: [number, number],
        layerID: number
    ) {
        const modelAltitude = 0;
        const modelRotate = [Math.PI / 2, 0, 0];

        let _camera = this.camera;
        let _scene = this.scene;
        let _renderer = this.renderer;
        let _map = this.map;

        var modelAsMercatorCoordinate = mapboxgl.MercatorCoordinate.fromLngLat(
            modelOrigin,
            modelAltitude
        );

        var modelTransform = {
            translateX: modelAsMercatorCoordinate.x,
            translateY: modelAsMercatorCoordinate.y,
            translateZ: modelAsMercatorCoordinate.z,
            rotateX: modelRotate[0],
            rotateY: modelRotate[1],
            rotateZ: modelRotate[2],
            scale: modelAsMercatorCoordinate.meterInMercatorCoordinateUnits()
        };

        let idName: string = `3d-model-${layerID}`;

        const customLayer: mapboxgl.AnyLayer = {
            id: idName,
            type: 'custom',
            renderingMode: '3d',
            onAdd: function (map: mapboxgl.Map, gl: WebGLRenderingContext) {

                // create two three.js lights to illuminate the model
                const directionalLight = new THREE.DirectionalLight(0xffffff);
                directionalLight.position.set(0, -70, 100).normalize();
                _scene.add(directionalLight);

                const directionalLight2 = new THREE.DirectionalLight(0xffffff);
                directionalLight2.position.set(0, 70, 100).normalize();
                _scene.add(directionalLight2);

                // use the three.js GLTF loader to add the 3D model to the three.js scene
                const loader = new GLTFLoader();
                loader.load(
                    'https://docs.mapbox.com/mapbox-gl-js/assets/34M_17/34M_17.gltf',
                    (gltf) => {
                        _scene.add(gltf.scene);
                    }
                );

                // use the Mapbox GL JS map canvas for three.js
                _renderer = new THREE.WebGLRenderer({
                    canvas: _map.getCanvas(),
                    context: gl,
                    antialias: true
                });

                _renderer.autoClear = false;
            },
            render: function (gl: WebGLRenderingContext, matrix: number[]) {
                const rotationX = new THREE.Matrix4().makeRotationAxis(
                    new THREE.Vector3(1, 0, 0),
                    modelTransform.rotateX
                );
                const rotationY = new THREE.Matrix4().makeRotationAxis(
                    new THREE.Vector3(0, 1, 0),
                    modelTransform.rotateY
                );
                const rotationZ = new THREE.Matrix4().makeRotationAxis(
                    new THREE.Vector3(0, 0, 1),
                    modelTransform.rotateZ
                );

                const m = new THREE.Matrix4().fromArray(matrix);

                // set model position
                const l = new THREE.Matrix4()
                    .makeTranslation(
                        modelTransform.translateX,
                        modelTransform.translateY,
                        Number(modelTransform.translateZ)
                    )
                    .scale(
                        new THREE.Vector3(
                            modelTransform.scale,
                            -modelTransform.scale,
                            modelTransform.scale
                        )
                    )
                    .multiply(rotationX)
                    .multiply(rotationY)
                    .multiply(rotationZ);

                _camera.projectionMatrix = m.multiply(l);
                _renderer.resetState();
                _renderer.render(_scene, _camera);
                _map.triggerRepaint();
            }
        };

        return customLayer;
    }

    constructor() {
        this.map = this.createMap();
        this.camera = new THREE.Camera();
        this.scene = new THREE.Scene();
        this.renderer = new THREE.WebGLRenderer();

        let raycaster = new THREE.Raycaster();

        this.map.on('style.load', () => {
            this.map.setConfigProperty('basemap', 'lightPreset', 'dusk');
            this.map.setConfigProperty('basemap', 'showPointOfInterestLabels', false);
            this.map.on('click', (e) => {
                var coords = e.lngLat;
                raycaster.setFromCamera(new THREE.Vector2(e.point.x, e.point.y), this.camera);
                let intersects = raycaster.intersectObjects(this.scene.children);
                // for (let i = 0; i < intersects.length; i++) {

                //     console.log(intersects[i].object.name);
                //     // do something
                //     //

                // }

                let modelLayer = this.createObjectOnCustomLayer([coords.lng, coords.lat], this.id);
                this.id += 1;
                this.map.addLayer(modelLayer);
            });
        });


    }

}

const newApp = new AppThreebox();
