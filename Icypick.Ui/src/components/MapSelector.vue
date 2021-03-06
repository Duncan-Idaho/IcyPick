<template>
  <div class="map-section">
    <MapButton v-for="map in maps" :key="map.id" :map="map" @click="onMapClick(map)"/>
    <MapButton @click="onMapClick(map)"/>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import MapButton from '@/components/MapButton.vue'
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
    MapButton,
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