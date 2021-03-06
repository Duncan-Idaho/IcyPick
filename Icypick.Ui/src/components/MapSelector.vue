<template>
  <div class="map-section">
    <Map v-for="map in maps" :key="map.id" :map="map"/>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import Map from '@/components/Map.vue'
import { maps } from '@/data.json'

export default defineComponent({
  name: 'MapSelector',
  components: {
    Map,
  },
  data() { 
    return {
      maps
    }
  }
})
</script>

<style lang="scss" scoped>

.map-section {
  width: 70%;
  display: flex;
  flex-wrap: wrap;
}

</style>