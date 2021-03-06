<template>
  <div class="main-area">
    <map-selector/>
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
  }
});
</script>

<style lang="scss" scoped>
.main-area {
  display: flex;
  justify-content: center;
}
</style>