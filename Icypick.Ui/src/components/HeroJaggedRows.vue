<template>
  <template v-for="(group, groupIndex) in groups" :key="groupIndex">
    <div class="long-line">
      <Hero v-for="hero in group.longLine" :key="hero.id" :hero-id="hero.id"/>
      <div class="gap" v-if="group.longLine.length % 2 !== 1"/>
    </div>
    <div class="short-line" v-if="group.shortLine.length">
      <Hero v-for="hero in group.shortLine" :key="hero.id" :hero-id="hero.id"/>
      <div class="gap" v-if="group.shortLine.length % 2 !== 0"/>
    </div>
  </template>
</template>

<script lang="ts">
import { defineComponent, PropType  } from 'vue';
import HeroView from './Hero.vue'
import { Hero } from '@/data'

export default defineComponent({
  name: 'HeroJaggedRows',
  props: {
    heroes: {
      type: Array as PropType<Hero[]>,
      required: true
    },
    rowSize: {
      type: Number,
      default: 7
    }
  },
  components: {
    Hero: HeroView,
  },
  computed: {
    groupRowSize(): number {
      return this.rowSize * 2 - 1
    },
    groups(): { longLine: Hero[]; shortLine: Hero[] }[] {
      return Array(Math.ceil(this.heroes.length / this.groupRowSize ))
        .fill(null)
        .map((_, rowGroup) => ({
          longLine: this.heroes.slice(
            rowGroup * this.groupRowSize, 
            rowGroup * this.groupRowSize + this.rowSize),
          shortLine: this.heroes.slice(
            rowGroup * this.groupRowSize + this.rowSize, 
            rowGroup * this.groupRowSize + this.rowSize + this.rowSize - 1)
        }))
    }
  }
});
</script>

<style lang="scss" scoped>
@use "sass:math";

$width: 5rem;
$height: $width / math.sqrt(3) * 2;
$rentrage: $height / 4;

.long-line, .short-line {
  font-size:0px;
}
.long-line + .short-line, .short-line + .long-line {
  margin-top: -$rentrage;
}
.short-line {
  margin-left: 2.5rem;
  margin-right: 2.5rem;
}
.gap {
  display:inline-block;
  width: $width;
}
</style>