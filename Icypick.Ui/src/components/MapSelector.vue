<template>
  <div class="map-section">
    <MapSlot v-for="map in maps" :key="map.id" :map="map" @click="onMapClick(map)"/>
    <MapSlot @click="onMapClick(map)"/>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import MapSlot from '@/components/MapSlot.vue'
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
    MapSlot,
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
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}

</style>