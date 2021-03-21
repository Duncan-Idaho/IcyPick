<template>
  <button v-if="bar" @click="click" class="map bar">
    <img v-if="map" :src="backgroundImage" draggable="false"/>
    <span v-else class="mdi mdi-help" />
    <span class="name">
      {{ map ? map.name : '' }}
    </span>
  </button>

  <button v-else-if="map" class="map tile" :style="{     
    backgroundImage: 
      `linear-gradient(rgba(0, 0, 0, 0), rgba(0, 0, 0, 0.3), rgba(0, 0, 0, 0.3), rgba(0, 0, 0, 0)), 
      url('${backgroundImage}')` 
  }" @click="click">
    <span>
      {{ map ? map.name : '' }}
    </span>
  </button>

  <button v-else class="map tile" @click="click">
    <span class="mdi mdi-help" />
  </button>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { Map } from '@/data'

export default defineComponent({
  name: 'MapButton',
  props: {
    map: {
      type: Object as PropType<Map>
    },
    bar: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    backgroundImage(): string | undefined {
      return this.map
        ? require(`@/assets/images/maps/${this.map.id}.jpg`)
        : undefined
    },
  }
})
</script>

<style lang="scss" scoped>
button.map {
  margin: 0.2rem;
  
  display: flex;
  justify-content:center; 
  align-items:center;

  font-size: 1.33rem;

  &.tile {
    width: 8rem;
    height: 8rem;
    background-size: cover;
    background-clip: padding-box;
  }

  &.tile .mdi {
    font-size: 5rem;
    width: 5rem
  }

  &.bar {
    width: 20rem;
    height: 3rem;

    & img {
      height: 100%;
      user-select: none;
    }
    & span.name {
      flex-grow: 1;
    }
    background-size: 3rem;
    background-repeat: no-repeat;
  }
}
</style>