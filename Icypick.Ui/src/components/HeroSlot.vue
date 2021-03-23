<template>
  <div :class="classes">
    <template v-if="hero">
      <div class="hero-border">
        <img class="hero-icon" 
          draggable="false"
          :src="require(`@/assets/images/heroes/${hero.id}.jpg`)" 
          :alt="hero.name" :title="hero.name"/>
      </div>
      <div 
        :class="{'hero-score': true, 'hero-score-positive': hero.score > 0, 'hero-score-negative': hero.score < 0 }">
        {{ hero.score }}
      </div>
      <div 
        :class="{'hero-ban-score': true, 'hero-score-positive': hero.banScore > 0, 'hero-score-negative': hero.banScore < 0 }">
        {{ hero.banScore }}
      </div>
      <div class="tooltip">
        <div class="hero-name">{{ hero.name }}</div>
        <div class="tooltip-content">
          <ul class="tooltip-main-score">
            <template v-for="reason in hero.scoreReasons" :key="reason.reason" >
              <li v-if="reason.score != 0" :class="{'hero-score-positive': reason.score > 0, 'hero-score-negative': reason.score < 0 }">
                <span class="hero-score-reason-increment">{{ (reason.score > 0 ? '+' : '\u2212') + Math.abs(reason.score) }}</span>
                <span class="hero-score-reason-text">{{ reason.reason }}</span>
              </li>
            </template>
          </ul>
          <ul class="tooltip-ban-score">
            <template v-for="reason in hero.banScoreReasons" :key="reason.reason" >
              <li v-if="reason.score != 0" :class="{'hero-score-positive': reason.score > 0, 'hero-score-negative': reason.score < 0 }">
                <span class="hero-score-reason-increment">{{ (reason.score > 0 ? '+' : '\u2212') + Math.abs(reason.score) }}</span>
                <span class="hero-score-reason-text">{{ reason.reason }}</span>
              </li>
            </template>
          </ul>
        </div>
      </div>
    </template>

    <div class="hero-border" v-else>
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
    },
    showScore: {
      type: Boolean,
      default: false
    },
    showBanScore: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    classes(): { [className: string]: boolean } {
      return {
        'hero-container': true,
        'show-score': this.showScore,
        'show-ban-score': this.showBanScore,
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

  .hero-score, .hero-ban-score {
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
  }

  .hero-score, .hero-ban-score {
    display: none;
  }

  &.show-score .hero-score {
    top: 100%;
    left: 50%;
    transform: translateX(-50%) translateY(-100%) translateX(calc(var(--hero-width, 5rem) / -4)) translateX(+0.2rem) translateY(-0.2rem);
    

    &.hero-score-positive {
      display: flex;

      background: rgb(41, 17, 85);
      color: lime;
    }

    &.hero-score-negative {
      display: flex;
      
      background: rgb(41, 17, 85);
      color: rgb(182, 4, 4);
    }
  }

  &.show-ban-score .hero-ban-score {
    top: 100%;
    left: 50%;
    transform: translateX(-50%) translateY(-100%) translateX(calc(var(--hero-width, 5rem) / 4)) translateX(-0.2rem) translateY(-0.2rem);
    

    &.hero-score-positive {
      display: flex;

      background: rgb(75, 16, 90);
      color: red;
    }

    &.hero-score-negative {
      display: flex;
      
      background: rgb(75, 16, 90);
      color: rgb(93, 93, 243);
    }
  }

  .tooltip {
    display: none;
  }

  &:hover.show-score .tooltip, &:hover.show-ban-score .tooltip {
    display: block;
    width: 10rem;
  }

  &:hover.show-score.show-ban-score .tooltip {
    display: block;
    width: 20rem;
  }

  &.show-score:hover .tooltip .tooltip-main-score {
    display:flex;
    flex-flow: column nowrap;
  }

  &.show-ban-score:hover .tooltip .tooltip-ban-score {
    display:flex;
    flex-flow: column nowrap;
  }

  &.show-score.show-ban-score:hover .tooltip .tooltip-ban-score {
    border-left: 1px solid black;
  }

  &:hover .tooltip {
    position: absolute;
    z-index: 10;

    top: 100%;
    left: 50%;
    transform: translateX(-50%);

    line-height: 1rem;

    padding: 0.8rem 0.4rem;
    border-radius: 0.4rem;
    
    $color: rgba(163, 161, 185, 0.8);

    background:$color;
    color: rgb(0, 0, 0);

    .hero-name {
      font-size: 1.3rem;
      font-weight: bolder;
      text-align: center;
      padding: 0.2rem 0rem 1rem 0rem;
    }

    .tooltip-content {
      display: flex;
    }

    ul {
      display: none;

      flex: 1 1 50%;
      margin: 0;
      padding: 0;
      text-align: left;
      list-style-type: none;
    }

    li {
      display: flex;
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

    .tooltip-main-score li {
      flex-flow: row nowrap;
      align-items: flex-start;
      justify-content: flex-start;
    }
    .tooltip-ban-score li {
      flex-flow: row-reverse nowrap;
      align-items: flex-start;
      justify-content: flex-start;
      text-align: right;
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
  .selected & {
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
