<template>
  <div class="layout">
    <div class="header">
      <MapSlot :map="selectedMap" bar @click="unselectMap"/>
    </div>
    <div class="body">
      <div class="allies-area">
      </div>
      <div class="main-area">
        <MapSelector v-model="selectedMap"/>
        <HeroSelector />
      </div>
      <div class="ennemies-area">
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HeroSelector from '@/components/HeroSelector.vue'
// import { maps } from '@/data.json'
import MapSelector from '@/components/MapSelector.vue';
import MapSlot from '@/components/MapSlot.vue';

export default defineComponent({
  name: 'Home',
  components: {
    HeroSelector,
    MapSlot,
    MapSelector
  },
  data() {
    return {
      selectedMap: null
    }
  },
  methods: {
    unselectMap() {
      this.selectedMap = null
    }
  }
});
</script>

<style lang="scss" scoped>*
.layout {
  display: flex;
  flex-flow: column nowrap;
  width: 100%;
  height: 100%;
}

.header {
  display: flex;
  justify-content: center;
}

.body {
  display: flex;
  flex-flow: row nowrap;
  flex: 1;
  overflow-y: scroll;
}

.allies-area, .ennemies-area {
  width: 40%;
}

.main-area {
  display: flex;
  flex-flow: column nowrap;
  align-items: center;
}
</style>