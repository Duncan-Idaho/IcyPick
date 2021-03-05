<template>
  <div :class="['hero-border', type || '']">
    <img
      :src="require(`@/assets/images/heroes/${hero.id}.jpg`)" 
      :alt="hero.name" :title="hero.name"/>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType  } from 'vue';
import { Hero } from '@/data'
import data from '@/data.json'

type type = 'ally' | 'ennemy' | undefined;

export default defineComponent({
  name: 'Hero',
  props: {
    heroId: {
      type: String,
      required: true
    },
    type: {
      type: String as PropType<type>,
      validator: (val: type) => !val || ['ally', 'ennemy'].includes(val)
    }
  },
  computed: {
    hero(): Hero {
      const hero = data.heroes.find(hero => hero.id === this.heroId)
      if (hero === undefined)
        throw new Error(`Hero id ${this.heroId} does not exist`);
      return hero;
    }
  }
});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
@use "sass:math";

@mixin hexagon($height) {
  clip-path: polygon(
    50% 0,
    100% 25%,
    100% 75%,
    50% 100%,
    0% 75%,
    0% 25%);
    
  $width: $height * math.sqrt(3) / 2;
  width: $width;
  height: $height;
}

@mixin hero-border($width, $border, $color) {
  display: inline-block;
  background-color: $color;

  position: relative;
  @include hexagon($width);

  img {
    object-fit: cover;

    position: absolute;
    left: $border;
    top: $border;
    @include hexagon($width - $border * 2);
  }
}

.hero-border {
  &.ally {
    @include hero-border(5rem, 0.3rem, rgb(99, 99, 209));  
  }
  &.ennemy {
    @include hero-border(5rem, 0.3rem, rgb(209, 99, 99));  
  }
  @include hero-border(5rem, 0.2rem, rgb(38, 38, 39));
}
</style>
