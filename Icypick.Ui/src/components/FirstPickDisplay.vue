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
  font-size: 1.33rem;

  color: white;
  font-weight: bold;

  margin: 0.2rem;
  border: 2px solid rgba(255, 255, 255, 0.7);
  width: 3rem;
  height: 3rem;
  
  // button reset
  background-color: transparent;
  box-sizing: content-box;
  outline: none;
  padding: 0;

  &:focus {
    border: 2px solid rgba(138, 150, 255, 1);
  }

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