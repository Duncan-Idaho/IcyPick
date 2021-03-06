<template>
  <div class="map-selected">
    <map-button :map="selectedMap" disabled bar/>
  </div>
  <div class="main-area">
    <map-selector v-model="selectedMap"/>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HeroSelector from '@/components/HeroSelector.vue'
// import { maps } from '@/data.json'
import MapSelector from '@/components/MapSelector.vue';
import MapButton from '@/components/Map.vue';

export default defineComponent({
  name: 'Home',
  components: {
    // HeroSelector,
    MapButton,
    MapSelector
  },
  data() {
    return {
      selectedMap: null
    }
  }
});
</script>

<style lang="scss" scoped>
.main-area {
  display: flex;
  justify-content: center;
}
.map-selected {
  display: flex;
  justify-content: center;
}
</style>