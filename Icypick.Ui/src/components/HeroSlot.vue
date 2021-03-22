<template>
  <div class="hero-container">
    <template v-if="hero">
      <div :class="classes">
        <img class="hero-icon" 
          draggable="false"
          :src="require(`@/assets/images/heroes/${hero.id}.jpg`)" 
          :alt="hero.name" :title="hero.name"/>
      </div>
      <div 
        :class="{'hero-score': true, 'hero-score-positive': hero.score > 0, 'hero-score-negative': hero.score < 0 }">
        {{ hero.score }}
      </div>
      <div class="tooltip">
        <div class="hero-name">{{ hero.name }}</div>
        <ul>
          <template v-for="reason in hero.scoreReasons" :key="reason.reason" >
            <li v-if="reason.score != 0" :class="{'hero-score-positive': reason.score > 0, 'hero-score-negative': reason.score < 0 }">
              <span class="hero-score-reason-increment">{{ (reason.score > 0 ? '+' : '\u2212') + Math.abs(reason.score) }}</span>
              <span class="hero-score-reason-text">{{ reason.reason }}</span>
            </li>
          </template>
        </ul>
      </div>
    </template>

    <div :class="classes" v-else>
      <span class="hero-icon mdi mdi-help" />
    </div>
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
        'hero-container': true,
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

.hero-container {
  display: inline-block;
  position: relative;

  .hero-score {
    display: none;
    position: absolute;

    justify-content: center;
    align-items: center;

    top: 100%;
    left: 50%;
    transform: translateX(-50%) translateY(-100%) translateX(calc(var(--hero-width, 5rem) / 4)) translateX(-0.2rem) translateY(-0.2rem);

    line-height: 1rem;
    width: 1.5rem;
    height: 1.5rem;

    border: 0.2rem solid rgb(38, 38, 39);
    border-radius: 1.5rem;
    padding: 0rem;
    
    font-weight: bold;

    z-index: 1;
    

    &.hero-score-positive {
      display: flex;

      background: rgb(41, 17, 85);
      color: lime;
    }

    &.hero-score-negative {
      display: flex;
      
      background: rgb(41, 17, 85);
      color: red;
    }
  }

  .tooltip {
    display: none;
  }

  &:hover .tooltip {
    display: block;
    position: absolute;
    z-index: 10;

    top: 100%;
    left: 50%;
    transform: translateX(-50%);

    width: 10rem;
    line-height: 1rem;

    padding: 0.4rem;
    border-radius: 0.4rem;
    
    $color: rgba(163, 161, 185, 0.8);

    background:$color;
    color: rgb(27, 27, 27);

    .hero-name {
      font-size: 1.3rem;
      font-weight: bolder;
      text-align: center;
      padding: 0.4rem 0rem;
    }

    ul {
      margin: 0;
      padding: 0;
      text-align: left;
      list-style-type: none;
    }

    li {
      display: flex;
      align-items: flex-start;
      justify-content: flex-start;
      padding: 0.2rem 0rem;

      &.hero-score-positive .hero-score-reason-increment {
        color: rgb(92, 250, 92);
        font-weight: bold;
      }

      &.hero-score-negative .hero-score-reason-increment {
        color: red;
        font-weight: bold;
      }
    }

    .hero-score-reason-increment {
      flex: none;
      width: 2rem;
      text-align: center;
    }

    &::after {
      content: "";
      position:absolute;

      top:0%;

      left:50%;
      transform:translateX(-50%) translateY(-100%);
      
      border: 0.5rem solid;
      border-color: transparent transparent $color transparent;
    }
  }
}

.hero-border {
  --hexagon-border: 0.2rem;
  background-color: rgb(38, 38, 39);
  &.selected {
    --hexagon-border: 0.2rem;
    background-color: rgba(138, 150, 255, 1);
  }

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
