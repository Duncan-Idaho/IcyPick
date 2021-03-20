<template>
  <div :class="classes">
    <img v-if="hero" class="hero-icon" draggable="false"
      :src="require(`@/assets/images/heroes/${hero.id}.jpg`)" 
      :alt="hero.name" :title="hero.name"/>
    <span v-else class="hero-icon mdi mdi-help" />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType  } from 'vue';
import { Hero } from '@/data'

export default defineComponent({
  name: 'HeroSlot',
  props: {
    hero: {
      type: Object as PropType<Hero | undefined>,
    },
    selected: {
      type: Boolean
    }
  },
  computed: {
    classes(): { [className: string]: boolean } {
      return {
        'hero-border': true,
        'selected': this.selected
      }
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
  &.selected {
    --hexagon-border: 0.2rem;
    background-color: rgba(138, 150, 255, 1);
  }

  display: inline-block;
  position: relative;
  --hexagon-width: #{$hero-width};

  .hero-icon {
    user-select: none;
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
    text-indent: 0;
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
