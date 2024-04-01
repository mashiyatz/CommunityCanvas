import mapboxgl from "mapbox-gl";
import * as THREE from "three";
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';
import { mapboxAccessToken } from '../src/global'

mapboxgl.accessToken = mapboxAccessToken;

class App {

    private map: mapboxgl.Map;

    private camera: THREE.Camera = new THREE.Camera();
    private scene: THREE.Scene = new THREE.Scene();
    private renderer: THREE.WebGLRenderer = new THREE.WebGLRenderer();

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
        modelOrigin: [number, number]
    ) {
        const modelAltitude = 0;
        const modelRotate = [Math.PI / 2, 0, 0];

        this.camera = new THREE.Camera();
        this.scene = new THREE.Scene();
        this.renderer = new THREE.WebGLRenderer();

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

        const customLayer: mapboxgl.AnyLayer = {
            id: '3d-model',
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

        this.map.on('style.load', () => {
            this.map.setConfigProperty('basemap', 'lightPreset', 'dusk');
            this.map.setConfigProperty('basemap', 'showPointOfInterestLabels', false);
            this.map.on('click', (e) => {
                var coords = e.lngLat;
                if (this.map.getLayer("3d-model")) {
                    this.map.removeLayer("3d-model");
                }
                if (this.map.getSource("3d-model")) {
                    this.map.removeSource("3d-model");
                }
                let modelLayer = this.createObjectOnCustomLayer([coords.lng, coords.lat]);
                this.map.addLayer(modelLayer);
            });
        });


    }
    
}

new App();