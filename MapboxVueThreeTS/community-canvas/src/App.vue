<script>
import Map from "./components/Map.vue";
import SideMenu from "./components/SideMenu.vue";
import "./assets/mapbox-gl.css";

export default {
  components: {
    Map,
    SideMenu
  }
};

</script>

<template>
  <SideMenu /> 
  <div id="layout">
    
    <Map/>
  </div>
</template>

<style lang="scss">
:root {
  --sidebar-width: 300px;
  --default-color: rgb(35 55 75 / 90%);
  --white-color: #fff;
}

* {
  font-family:'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;
}

#layout {
  flex: 1;
  display: flex;
}
</style>