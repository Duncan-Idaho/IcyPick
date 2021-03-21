<template>
  <div class="role-sections">
    <button @click="onUnselect">Unselect</button>
    <div class="role-section" v-for="role in roles" :key="role.id">
      <img class="role" :src="require(`@/assets/${role.id}.png`)" draggable="false" />
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

interface Role {
  id: string;
  heroes: Hero[];
}

export default defineComponent({
  name: 'HeroSelector',
  props: {
    modelValue: {
      type: Object as PropType<Hero | undefined>
    },
    heroes: {
      type: Array as PropType<Hero[]>,
      required: true
    }
  },
  emits: ['update:modelValue'],
  components: {
    HeroJaggedRows,
  },
  computed: {
    roles(): Role[] {
      const modelValue = this.modelValue

      const roles = [...new Set(this.heroes.map(hero => hero.role))]
      return roles.map(role => {
        const heroesInRole: Hero[] = this.heroes.filter(hero => hero.role === role)
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
      this.$emit('update:modelValue', role.heroes[index])
    },
    onUnselect() {
      this.$emit('update:modelValue', undefined)
    }
  }
})
</script>

<style lang="scss" scoped>
.role-sections {
  user-select: none;
  display: flex;
  flex-flow: column nowrap;
  align-items: flex-start;
}

button {
  width: 100%
}

.role-section {
  text-align: center;
  padding: 0.5rem;

  display: flex;
  flex-wrap: nowrap;
  align-items: center;
}
</style>