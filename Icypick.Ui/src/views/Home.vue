<template>
  <div class="main-area">
    <hero-selector/>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HeroSelector from '@/components/HeroSelector.vue'

export default defineComponent({
  name: 'Home',
  components: {
    HeroSelector,
  }
});
</script>

<style lang="scss" scoped>
.main-area {
  display: flex;
  justify-content: center;
}
</style>