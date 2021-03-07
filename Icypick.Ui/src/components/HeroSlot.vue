<template>
  <div :class="['hero-border', type || '']">
    <img v-if="hero" class="hero-icon"
      :src="require(`@/assets/images/heroes/${hero.id}.jpg`)" 
      :alt="hero.name" :title="hero.name"/>
    <span v-else class="hero-icon mdi mdi-help" />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType  } from 'vue';
import { Hero } from '@/data'

type type = 'ally' | 'ennemy' | undefined;

export default defineComponent({
  name: 'HeroSlot',
  props: {
    hero: {
      type: Object as PropType<Hero | undefined>,
    },
    type: {
      type: String as PropType<type>,
      validator: (val: type) => !val || ['ally', 'ennemy'].includes(val)
    }
  }
});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
@use "sass:math";

$hero-width: var(--hero-width, 5rem);

.hero-border {
  --hexagon-border: 0.2rem;
  background-color: rgb(38, 38, 39);
  &.ally {
    --hexagon-border: 0.3rem;
    background-color: rgb(99, 99, 209);
  }
  &.ennemy {
    --hexagon-border: 0.3rem;
    background-color: rgb(209, 99, 99);
  }

  display: inline-block;
  position: relative;
  --hexagon-width: #{$hero-width};

  .hero-icon {
    object-fit: cover;
    background-color: #1a0a38;
    color: white;

    position: absolute;
    left: var(--hexagon-border);
    top: var(--hexagon-border);
    --hexagon-width: calc(#{$hero-width} - var(--hexagon-border) * 2);
    
    // for mdi icon
    text-align: center;
    font-size: calc(#{$hero-width} * 0.5);
  }

  // Hexagon shape
  &, .hero-icon {
    clip-path: polygon(
      50% 0,
      100% 25%,
      100% 75%,
      50% 100%,
      0% 75%,
      0% 25%);

    width: var(--hexagon-width);

    $height: calc(var(--hexagon-width) / #{math.sqrt(3)} * 2);
    height: $height; // for image
    line-height: $height; // for mdi icon
  }

}
</style>
