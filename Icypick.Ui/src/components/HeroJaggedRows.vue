<template>
  <template v-for="(group, groupIndex) in groups" :key="groupIndex">
    <div class="long-line">
      <HeroSlot v-for="hero in group.longLine" :key="hero.id" :hero-id="hero.id"/>
      <div class="gap" v-if="rowSize != 1 && group.longLine.length % 2 !== 1"/>
    </div>
    <div class="short-line" v-if="group.shortLine.length">
      <HeroSlot v-for="hero in group.shortLine" :key="hero.id" :hero-id="hero.id"/>
      <div class="gap" v-if="rowSize != 1 && group.shortLine.length % 2 !== 0"/>
    </div>
  </template>
</template>

<script lang="ts">
import { defineComponent, PropType  } from 'vue';
import HeroSlot from './HeroSlot.vue'
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
    HeroSlot,
  },
  computed: {
    groupRowSize(): number {
      return this.rowSize * 2 - 1
    },
    groups(): { longLine: Hero[]; shortLine: Hero[] }[] {
      if (this.rowSize === 1) {
        return Array(Math.ceil(this.heroes.length / 2 ))
          .fill(null)
          .map((_, rowGroup) => ({
            longLine: this.heroes.slice(
              rowGroup * 2, 
              rowGroup * 2 + 1),
            shortLine: this.heroes.slice(
              rowGroup * 2 + 1, 
              rowGroup * 2 + 2)
          }))
      }

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