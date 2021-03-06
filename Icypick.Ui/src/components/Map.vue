<template>
  <div class="map" :style="{     
    backgroundImage: 
      `linear-gradient(rgba(0, 0, 0, 0), rgba(0, 0, 0, 0.3), rgba(0, 0, 0, 0.3), rgba(0, 0, 0, 0)), 
      url('${backgroundImage}')` 
  }">
    <span>
      {{ map.name }}
    </span>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { Map } from '@/data'

export default defineComponent({
  name: 'Map',
  props: {
    map: {
      type: Object as PropType<Map>,
      required: true
    }
  },
  computed: {
    backgroundImage(): string {
      return require(`@/assets/images/maps/${this.map.id}.jpg`)
    },
  }
})
</script>

<style lang="scss" scoped>
.map {
  background-size: cover;
  width: 8rem;
  height: 8rem;
  margin: 0.2rem;
  border: 2px solid rgba(255, 255, 255, 0.7);
  background-clip: padding-box;
  
  display: flex;
  justify-content:center; 
  align-items:center;

  color: white;
  font-weight: bold;
  font-size: 1.33rem;
}
</style>