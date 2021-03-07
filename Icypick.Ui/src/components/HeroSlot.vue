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
  name: 'HeroSlot',
  props: {
    hero: {
      type: Object as PropType<Hero>,
      required: true
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

  img {
    object-fit: cover;

    position: absolute;
    left: var(--hexagon-border);
    top: var(--hexagon-border);
    --hexagon-width: calc(#{$hero-width} - var(--hexagon-border) * 2);
  }

  // Hexagon shape
  &, img {
    clip-path: polygon(
      50% 0,
      100% 25%,
      100% 75%,
      50% 100%,
      0% 75%,
      0% 25%);

    width: var(--hexagon-width);
    height: calc(var(--hexagon-width) / #{math.sqrt(3)} * 2);
  }

}
</style>
