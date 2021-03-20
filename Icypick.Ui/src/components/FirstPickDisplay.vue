<template>
  <button :class="[ ...selectedClass ]"/>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { FirstPick } from '@/domain/order'

export default defineComponent({
  name: 'FirstPickDisplay',
  props: {
    modelValue: {
      type: String as PropType<FirstPick>
    }
  },
  computed: {
    selectedClass(): string[] {
      if (!this.modelValue)
        return ['mdi', 'mdi-help']

      return [ this.modelValue ]
    }
  }
})
</script>

<style lang="scss" scoped>

button {
  margin: 0.2rem;
  width: 3rem;
  height: 3rem;
  
  &.ally {
    &::before {
      content: '1'
    }
  }
  &.ennemy {
    &::before {
      content: '2'
    }
  }
}

</style>