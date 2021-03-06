<template>
  <div class="layout">
    <div class="header">
      <MapSlot :map="selectedMap" bar @click="unselectMap"/>
    </div>
    <div class="body">
      <div class="allies-area">
        <HeroJaggedRows :heroes="allies" :row-size="1"/>
      </div>
      <div class="main-area">
        <MapSelector v-model="selectedMap"/>
        <HeroSelector />
      </div>
      <div class="ennemies-area">
        <HeroJaggedRows :heroes="ennemies" :row-size="1"/>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import HeroSelector from '@/components/HeroSelector.vue'
import { heroes } from '@/data.json'
import MapSelector from '@/components/MapSelector.vue';
import MapSlot from '@/components/MapSlot.vue';
import HeroJaggedRows from '@/components/HeroJaggedRows.vue';

export default defineComponent({
  name: 'Home',
  components: {
    HeroSelector,
    MapSlot,
    MapSelector,
    HeroJaggedRows
  },
  data() {
    return {
      selectedMap: null,
      allies: heroes.slice(0, 5),
      ennemies: heroes.slice(5, 10)
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
  flex: 0 0 15rem;
}

.allies-area {
  text-align: left;
}

.ennemies-area {
  text-align: right;
}

.main-area {
  display: flex;
  flex-flow: column nowrap;
  align-items: center;
}
</style>