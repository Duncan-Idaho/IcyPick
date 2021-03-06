<template>
  <div class="map-section">
    <Map v-for="map in maps" :key="map.id" :map="map" @click="onMapClick(map)" readonly/>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import MapButton from '@/components/Map.vue'
import { Map } from '@/data'
import { maps } from '@/data.json'

export default defineComponent({
  name: 'MapSelector',
  props: {
    modelValue: {
      type: Object as PropType<Map>
    }
  },
  emits: ['update:modelValue'],
  components: {
    Map: MapButton,
  },
  data() { 
    return {
      maps
    }
  },
  methods: {
    onMapClick(map: Map) {
      this.$emit('update:modelValue', map)
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