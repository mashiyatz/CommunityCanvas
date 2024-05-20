import { defineStore } from 'pinia';

interface State {
    isSelected: boolean
}

export const selectionStore = defineStore('selected', {    
    state: (): State => { 
        return {
            isSelected: false
        }  
    }
  })