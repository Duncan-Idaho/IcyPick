<template>
  <div class="role-sections">
    <div class="role-section" v-for="role in roles" :key="role.id">
      <img class="role" :src="require(`@/assets/${role.id}.png`)" />
      <div>
        <hero-jagged-rows :heroes="role.heroes" :rowSize="7"/>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HeroJaggedRows from '@/components/HeroJaggedRows.vue'
import { heroes } from '@/data.json'

export default defineComponent({
  name: 'HeroSelector',
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
})
</script>

<style lang="scss" scoped>
.role-sections {
  display: flex;
  flex-flow: column nowrap;
  align-items: flex-start;
}

.role-section {
  text-align: center;
  padding: 0.5rem;

  display: flex;
  flex-wrap: nowrap;
  align-items: center;
}
</style>