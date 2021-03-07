<template>
  <div class="role-sections">
    <div class="role-section" v-for="role in roles" :key="role.id">
      <img class="role" :src="require(`@/assets/${role.id}.png`)" />
      <div>
        <HeroJaggedRows 
          :heroes="role.heroes" 
          :row-size="7"
          :modelValue="role.selectedIndex" 
          @update:modelValue="onHeroClick(role, $event)"/>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import HeroJaggedRows from '@/components/HeroJaggedRows.vue'
import { Hero } from '@/data'
import { heroes } from '@/data.json'

interface Role {
  id: string;
  heroes: Hero[];
}

export default defineComponent({
  name: 'HeroSelector',
  props: {
    modelValue: {
      type: Object as PropType<Hero | undefined>
    }
  },
  emits: ['update:modelValue'],
  components: {
    HeroJaggedRows,
  },
  computed: {
    roles(): Role[] {
      const modelValue = this.modelValue;
      const roles = [...new Set(heroes.map(hero => hero.role))]
      return roles.map(role => {
        const heroesInRole: Hero[] = heroes.filter(hero => hero.role === role)
        const selectedIndex = modelValue
          ? heroesInRole.findIndex(hero => hero.id === modelValue.id)
          : -1
        return {
          id: role,
          heroes: heroesInRole,
          selectedIndex: selectedIndex >= 0 ? selectedIndex : undefined
        }
      })
    }
  },
  methods: {
    onHeroClick(role: Role, index: number) {
      console.log(role.heroes[index])
      this.$emit('update:modelValue', role.heroes[index])
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