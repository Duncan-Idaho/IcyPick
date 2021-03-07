<template>
  <template v-for="(group, groupIndex) in groups" :key="groupIndex">
    <div class="long-line">
      <HeroSlot v-for="{ hero, index } in group.longLine" 
        :key="index" 
        :hero="hero" 
        :selected="modelValue === index"
        @click="onHeroClick(index)"/>

      <div class="gap" v-if="rowSize != 1 && group.longLine.length % 2 !== 1"/>
    </div>
    <div class="short-line" v-if="group.shortLine.length">
      <HeroSlot v-for="{ hero, index } in group.shortLine" 
        :key="index" 
        :hero="hero" 
        :selected="modelValue === index"
        @click="onHeroClick(index)"/>

      <div class="gap" v-if="rowSize != 1 && group.shortLine.length % 2 !== 0"/>
    </div>
  </template>
</template>

<script lang="ts">
import { defineComponent, PropType  } from 'vue';
import HeroSlot from './HeroSlot.vue'
import { Hero } from '@/data'

interface HeroSlot {
  index: number;
  hero: Hero | null;
}

interface Group {
  longLine: HeroSlot[]; 
  shortLine: HeroSlot[];
}

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
    },
    modelValue: {
      type: Number as PropType<number | undefined>
    }
  },
  emits: ['update:modelValue'],
  components: {
    HeroSlot,
  },
  computed: {
    groupRowSize(): number {
      return this.rowSize * 2 - 1
    },
    indexHeroes(): HeroSlot[] {
      return this.heroes.map((hero, index) => ({
        hero, index
      }))
    },
    groups(): Group[] {
      if (this.rowSize === 1) {
        return Array(Math.ceil(this.indexHeroes.length / 2 ))
          .fill(null)
          .map((_, rowGroup) => ({
            longLine: this.indexHeroes.slice(
              rowGroup * 2, 
              rowGroup * 2 + 1),
            shortLine: this.indexHeroes.slice(
              rowGroup * 2 + 1, 
              rowGroup * 2 + 2)
          }))
      }

      return Array(Math.ceil(this.indexHeroes.length / this.groupRowSize ))
        .fill(null)
        .map((_, rowGroup) => ({
          longLine: this.indexHeroes.slice(
            rowGroup * this.groupRowSize, 
            rowGroup * this.groupRowSize + this.rowSize),
          shortLine: this.indexHeroes.slice(
            rowGroup * this.groupRowSize + this.rowSize, 
            rowGroup * this.groupRowSize + this.rowSize + this.rowSize - 1)
        }))
    }
  },
  methods: {
    onHeroClick(index: number) {
      this.$emit('update:modelValue', index)
    }
  }
});
</script>

<style lang="scss" scoped>
@use "sass:math";

$hero-width: var(--hero-width, 5rem);
$hero-height: calc(#{$hero-width} / #{math.sqrt(3)} * 2);

$interlocking-height: calc(#{$hero-height} / 4);
$shift: calc(#{$hero-width} / 2);

.long-line, .short-line {
  line-height: 0px;
}
.long-line + .short-line, .short-line + .long-line {
  margin-top: calc(-1 * #{$interlocking-height});
}
.short-line {
  margin-left: $shift;
  margin-right: $shift;
}
.gap {
  display:inline-block;
  width: $hero-width;
}
</style>