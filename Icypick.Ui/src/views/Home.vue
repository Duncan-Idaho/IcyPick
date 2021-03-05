<template>
  <div class="central" v-for="category in categories" :key="category.id">
    <h1>{{ category.id }} ({{category.heroes.length }})</h1>
    <div>
    <hero-jagged-rows :heroes="category.heroes" :rowSize="7"/>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HeroJaggedRows from '@/components/HeroJaggedRows.vue'
import { heroes } from '@/data.json'

export default defineComponent({
  name: 'Home',
  components: {
    HeroJaggedRows,
  },
  data() { 
    const categories = [...new Set(heroes.map(hero => hero.category))]
    return {
      categories: categories.map(category => ({
        id: category,
        heroes: heroes.filter(hero => hero.category === category)
      })),
      heroes: heroes
    }
  }
});
</script>

<style lang="scss" scoped>
.central {
  width: 80%;
  margin: auto;
  border: 1px solid black;
}
</style>