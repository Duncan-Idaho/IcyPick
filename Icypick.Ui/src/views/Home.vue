<template>
  <h1 v-if="selectedMap">{{ selectedMap.name }}</h1>
  <h1 v-else>Please select a map</h1>
  <div class="main-area">
    <map-selector v-model="selectedMap"/>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HeroSelector from '@/components/HeroSelector.vue'
// import { maps } from '@/data.json'
import MapSelector from '@/components/MapSelector.vue';

export default defineComponent({
  name: 'Home',
  components: {
    // HeroSelector,
    MapSelector,
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
</style>