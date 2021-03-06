<template>
  <div class="central" v-for="role in roles" :key="role.id">
    <h1>{{ role.id }} ({{role.heroes.length }})</h1>
    <div>
    <hero-jagged-rows :heroes="role.heroes" :rowSize="7"/>
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
    const roles = [...new Set(heroes.map(hero => hero.role))]
    return {
      roles: roles.map(role => ({
        id: role,
        heroes: heroes.filter(hero => hero.role === role)
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