<template>
  <div class="layout">
    <div class="header">
      <div class="ally-bans-area">
        <HeroJaggedRows 
          :heroes="allyBans" 
          :row-size="3"
          :modelValue="selectedAllyBanSlot"
          @update:modelValue="onAllyBanSlotClicked"
          />
      </div>
      <div class="map-area">
        <MapSlot :map="selectedMap" bar @click="unselectMap"/>
      </div>
      <div class="ennemy-bans-area">
        <HeroJaggedRows 
          :heroes="ennemyBans" 
          :row-size="3"
          :modelValue="selectedEnnemyBanSlot"
          @update:modelValue="onEnnemyBanSlotClicked"
          /></div>
    </div>
    <div class="body">
      <div class="allies-area">
        <HeroJaggedRows 
          :heroes="allies" 
          :row-size="1"
          alignment="left"
          :modelValue="selectedAllySlot"
          @update:modelValue="onAllySlotClicked"
          />
      </div>
      <div class="main-area">
        <MapSelector v-if="selectedSlot === 'map'" v-model="selectedMap"/>
        <HeroSelector v-if="!!selectedSlot && !!selectedSlot.kind" v-model="selectedHero"/>
      </div>
      <div class="ennemies-area">
        <HeroJaggedRows 
          :heroes="ennemies" 
          :row-size="1"
          alignment="right"
          :modelValue="selectedEnnemySlot"
          @update:modelValue="onEnnemySlotClicked"
          />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, watch, reactive, toRefs } from 'vue';
import HeroSelector from '@/components/HeroSelector.vue'
import MapSelector from '@/components/MapSelector.vue';
import MapSlot from '@/components/MapSlot.vue';
import HeroJaggedRows from '@/components/HeroJaggedRows.vue';
import { Map, Hero } from '@/data';
import { SlotId, GenericSlotId, getIndexFor, isHeroSlot } from '@/domain/order'

interface Data {
  selectedMap: Map | null;
  allies: (Hero | undefined)[];
  ennemies: (Hero | undefined)[];
  allyBans: (Hero | undefined)[];
  ennemyBans: (Hero | undefined)[];
  selectedSlot: SlotId | null;
}

export default defineComponent({
  name: 'Home',
  components: {
    HeroSelector,
    MapSlot,
    MapSelector,
    HeroJaggedRows
  },
  setup() {
    const data = reactive<Data>({
      selectedMap: null,
      allies: Array(Math.ceil(5)).fill(undefined),
      ennemies: Array(Math.ceil(5)).fill(undefined),
      allyBans: Array(Math.ceil(3)).fill(undefined),
      ennemyBans: Array(Math.ceil(3)).fill(undefined),
      selectedSlot: GenericSlotId.Map
    })

    const selectedHeroRow = computed(() => {
      if (!data.selectedSlot || typeof data.selectedSlot === 'string')
        return undefined

      switch (data.selectedSlot.kind)
      {
        case 'ally':
          return data.allies
        case 'ennemy':
          return data.ennemies
        case 'allyBan':
          return data.allyBans
        case 'ennemyBan':
          return data.ennemyBans
        default:
          return undefined
      }
    })

    const selectedHero = computed({
      get(): Hero | undefined {
        if (!selectedHeroRow.value || !isHeroSlot(data.selectedSlot))
          return undefined;
        return selectedHeroRow.value[data.selectedSlot.index]
      },
      set(value: Hero |  undefined) {
        if (!selectedHeroRow.value || !isHeroSlot(data.selectedSlot))
          return;

        selectedHeroRow.value[data.selectedSlot.index] = value
      }
    })

    const selectNextSlot = () => {
      if (!data.selectedMap) {
        data.selectedSlot = GenericSlotId.Map
      } else {
        let firstAllyNotSelected = data.allies.indexOf(undefined);
        let firstEnnemyNotSelected = data.ennemies.indexOf(undefined);

        if (firstAllyNotSelected === -1)
          firstAllyNotSelected = 5;
        if (firstEnnemyNotSelected === -1)
          firstEnnemyNotSelected = 5;

        data.selectedSlot = { 
          kind: firstAllyNotSelected <= firstEnnemyNotSelected ? 'ally' : 'ennemy',
          index: Math.min(firstAllyNotSelected, firstEnnemyNotSelected)
        };

        if (data.selectedSlot.index ===5)
          data.selectedSlot = null
      }
    }

    watch(() => data.allies, selectNextSlot, { deep: true});
    watch(() => data.ennemies, selectNextSlot, { deep: true});
    watch(() => data.allyBans, selectNextSlot, { deep: true});
    watch(() => data.ennemyBans, selectNextSlot, { deep: true});
    watch(() => data.selectedMap, selectNextSlot, { deep: true});

    return {
      ...toRefs(data),
      selectedHeroRow,
      selectedHero,

      selectedAllySlot: computed(() => getIndexFor(data.selectedSlot, 'ally')),
      selectedEnnemySlot: computed(() => getIndexFor(data.selectedSlot, 'ennemy')),
      selectedAllyBanSlot: computed(() => getIndexFor(data.selectedSlot, 'allyBan')),
      selectedEnnemyBanSlot: computed(() => getIndexFor(data.selectedSlot, 'ennemyBan')),      
      
      unselectMap: () => data.selectedMap = null,
      selectSlot: (slotId: SlotId) => data.selectedSlot = slotId,
      onAllySlotClicked: ( index: number ) => data.selectedSlot = { kind: 'ally', index },
      onEnnemySlotClicked: ( index: number ) => data.selectedSlot = { kind: 'ennemy', index },
      onAllyBanSlotClicked: ( index: number ) => data.selectedSlot = { kind: 'allyBan', index },
      onEnnemyBanSlotClicked: ( index: number ) => data.selectedSlot = { kind: 'ennemyBan', index },

      selectNextSlot,
    }
  }
});
</script>

<style lang="scss" scoped>*
.layout {
  width: 100%;
  height: 100%;
  
  display: flex;
  flex-flow: column nowrap;
}

.header {
  display: flex;
  justify-content: space-around;
}

.body {
  flex: 1;
  overflow: hidden;
  
  display: flex;
  flex-flow: row nowrap;
}

.allies-area, .ennemies-area {
  --hero-width: 10rem;
}

.allies-area {
  margin-right: 0.5em;
}

.ennemies-area {
  margin-left: 0.5em;
}

.main-area {
  flex: 1;
  overflow: auto;

  display: flex;
  flex-flow: column nowrap;
  align-items: center;  
}
</style>