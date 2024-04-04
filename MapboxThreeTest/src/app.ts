import mapboxgl from "mapbox-gl";
import * as THREE from "three";
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
import { mapboxAccessToken } from '../src/global'
import { Threebox } from 'threebox-plugin';

mapboxgl.accessToken = mapboxAccessToken;

let styles = {
    day: 'satellite-streets-v9',
    night: 'dark-v10'
}
let selectedStyle = styles.day;

var map = new mapboxgl.Map({
    container: 'map',
    style: 'mapbox://styles/mapbox/' + selectedStyle,
    zoom: 18,
    center: [-73.8892669226548, 40.753826358076516],
    pitch: 60,
    antialias: true // create the gl context with MSAA antialiasing, so custom layers are antialiased
});

var tb = new Threebox(
    map,
    map.getCanvas().getContext('webgl'),
    {
        realSunlight: true,
        sky: true,
        enableSelectingObjects: true,
        enableTooltips: true
    }
)

// let stats: Stats;

class AppThreebox {

    private createCustomLayer(layerName: string, origin: [number, number]) {

        let model;
        //create the layer
        let customLayer3D: mapboxgl.AnyLayer = {
            id: layerName,
            type: 'custom',
            renderingMode: '3d',
            onAdd: function (map: mapboxgl.Map, gl: WebGLRenderingContext) {
                // Attribution, no License specified: Model by https://github.com/nasa/
                // https://nasa3d.arc.nasa.gov/detail/jpl-vtad-dsn34
                let options = {
                    type: 'gltf', //'gltf'/'mtl'
                    obj: 'https://docs.mapbox.com/mapbox-gl-js/assets/34M_17/34M_17.gltf', //model url
                    units: 'meters', //units in the default values are always in meters
                    scale: 333.22,
                    rotation: { x: 90, y: 180, z: 0 }, //default rotation
                    anchor: 'center'
                }

                // what is model's type?
                tb.loadObj(options, function (model: any) {
                    model.setCoords(origin);
                    model.addTooltip("A radar in the middle of nowhere", true);
                    tb.add(model);
                    model.castShadow = true;
                    tb.lights.dirLight.target = model;
                });

            },
            render: function (gl: WebGLRenderingContext, matrix: number[]) {
                /*                tb.setSunlight(date, origin); //set Sun light for the given datetime and lnglat
                                let dupDate = new Date(date.getTime()); // dup the date to avoid modify the original instance
                                let dateTZ = new Date(dupDate.toLocaleString("en-US", { timeZone: 'Australia/Sydney' }));
                                hour.innerHTML = "Sunlight on date/time: " + dateTZ.toLocaleString();*/
                tb.update();
            }
        };
        return customLayer3D;
    };

    private animate(): void {
        requestAnimationFrame(this.animate);
        /*stats.update();*/
    }

    constructor() {

        let model;
        let modelOrigin: [number, number] = [-73.8892669226548, 40.753826358076516];

        let date = new Date();//new Date(2020, 7, 14, 0, 39); // change this UTC date time to show the shadow view
        let time = date.getHours() * 3600 + date.getMinutes() * 60 + date.getSeconds();
        /*        let timeInput = document.getElementById('time');
                timeInput.value = time;
                timeInput.oninput = () => {
                    time = +timeInput.value;
                    date.setHours(Math.floor(time / 60 / 60));
                    date.setMinutes(Math.floor(time / 60) % 60);
                    date.setSeconds(time % 60);
                    map.triggerRepaint();
                };*/

        /*let stats;*/

        map.on('style.load', () => {
            /*stats = new Stats();
            map.getContainer().appendChild(stats.dom);*/
            this.animate();
            map.addLayer(this.createCustomLayer('3d-model', modelOrigin));

            map.on('click', (e) => {
                
                let options = {
                    type: 'gltf', //'gltf'/'mtl'
                    obj: 'https://docs.mapbox.com/mapbox-gl-js/assets/34M_17/34M_17.gltf', //model url
                    units: 'meters', //units in the default values are always in meters
                    scale: 333.22,
                    rotation: { x: 90, y: 180, z: 0 }, //default rotation
                    anchor: 'center'
                }
                tb.loadObj(options, function (model: any) {
                    model.setCoords(origin);
                    model.addTooltip("A radar in the middle of nowhere", true);
                    tb.add(model);
                    model.castShadow = true;
                    tb.lights.dirLight.target = model;
                });
            });
            
        });
    }
}


/*class App {

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
                *//*                if (this.map.getLayer("3d-model")) {
                                    this.map.removeLayer("3d-model");
                                }
                                if (this.map.getSource("3d-model")) {
                                    this.map.removeSource("3d-model");
                                }*//*
                raycaster.setFromCamera(new THREE.Vector2(e.point.x, e.point.y), this.camera);
                let intersects = raycaster.intersectObjects(this.scene.children);
                for (let i = 0; i < intersects.length; i++) {

                    console.log(intersects[i].object.name);
                    // do something
                    //

                }

                let modelLayer = this.createObjectOnCustomLayer([coords.lng, coords.lat], this.id);
                this.id += 1;
                this.map.addLayer(modelLayer);
            });
        });


    }

}*/

new AppThreebox();