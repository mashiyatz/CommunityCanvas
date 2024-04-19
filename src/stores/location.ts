import { defineStore } from 'pinia';

interface State {
    lng: number,
    lat: number,
    bearing: number,
    pitch: number,
    zoom: number,
}

export const locationStore = defineStore('location', {    
    state: (): State => { 
        return {
            lng: -73.8892669226548, 
            lat: 40.753826358076516,
            bearing: 0,
            pitch: 60,
            zoom: 20,
        }  
    }
  })