<template>
  <div class="pick-section">
    <button @click="onFirstPickClick">First Pick</button>
    <button @click="onSecondPickClick">Second Pick</button>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { FirstPick } from '@/domain/order'

export default defineComponent({
  name: 'PickSelector',
  props: {
    modelValue: {
      type: String as PropType<FirstPick>
    }
  },
  emits: ['update:modelValue'],
  computed: {
    isFirstPickSelected(): boolean {
      return this.modelValue === 'ally'
    },
    isSecondPickSelected(): boolean {
      return this.modelValue === 'ally'
    }
  },
  methods: {
    onFirstPickClick() {
      this.$emit('update:modelValue', 'ally')
    },
    onSecondPickClick() {
      this.$emit('update:modelValue', 'ennemy')
    }
  }
})
</script>

<style lang="scss" scoped>

.pick-section {
  display: flex;
  flex-flow: column nowrap;

  button {
    font-size: 3rem;

    margin: 4rem;
    padding: 2rem 3rem;
  }
}

</style>