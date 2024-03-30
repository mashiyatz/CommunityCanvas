import mapboxgl from "mapbox-gl";

mapboxgl.accessToken = global.accessToken;

class App {

    private createMap() {
        let bounds: [number, number, number, number] = [-73.96531962759687, 40.71650398136544, -73.85090773503727, 40.77187154866687];
        const map = new mapboxgl.Map({
            container: 'map',
            style: 'mapbox://styles/mapbox/standard', 
            center: [-73.8892669226548, 40.753826358076516],
            zoom: 20,
            maxBounds: bounds
          });

        map.on('style.load', () => {
            map.setConfigProperty('basemap', 'lightPreset', 'dusk');
            map.setConfigProperty('basemap', 'showPointOfInterestLabels', false);
        });
    }

    constructor() {
        this.createMap();
    }
    
}

new App();