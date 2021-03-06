<template>
  <div class="map-selected">
    <MapSlot :map="selectedMap" bar @click="unselectMap"/>
  </div>
  <div class="main-area">
    <MapSelector v-model="selectedMap"/>
  </div>
  <div class="main-area">
    <HeroSelector />
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HeroSelector from '@/components/HeroSelector.vue'
// import { maps } from '@/data.json'
import MapSelector from '@/components/MapSelector.vue';
import MapSlot from '@/components/MapSlot.vue';

export default defineComponent({
  name: 'Home',
  components: {
    HeroSelector,
    MapSlot,
    MapSelector
  },
  data() {
    return {
      selectedMap: null
    }
  },
  methods: {
    unselectMap() {
      this.selectedMap = null
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