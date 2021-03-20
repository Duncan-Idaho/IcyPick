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
        <button class="reset-button mdi mdi-undo-variant" @click="reset"/>
        <MapSlot :map="selectedMap" bar @click="unselectMap"/>
        <FirstPickDisplay :modelValue="firstPick" @click="unsetFirstPick" />
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
        <FirstPickSelector v-if="selectedSlot === 'pick'" v-model="firstPick"/>
        <HeroSelector v-if="!!selectedSlot && !!selectedSlot.kind" v-model="selectedHero" :heroes="availableHeroes"/>
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
import { computed, defineComponent, watch, reactive, toRefs } from 'vue'
import HeroSelector from '@/components/HeroSelector.vue'
import MapSelector from '@/components/MapSelector.vue'
import MapSlot from '@/components/MapSlot.vue'
import HeroJaggedRows from '@/components/HeroJaggedRows.vue'
import FirstPickSelector from '@/components/FirstPickSelector.vue'
import FirstPickDisplay from '@/components/FirstPickDisplay.vue'
import { heroes } from '@/data.json'
import { Map, Hero } from '@/data'
import { SlotId, GenericSlotId, getIndexFor, createSlots, isHeroSlot, FirstPick } from '@/domain/order'

interface Data {
  selectedMap: Map | null;
  firstPick: FirstPick;
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
    HeroJaggedRows,
    FirstPickSelector,
    FirstPickDisplay
  },
  setup() {
    function getResetData() {
      return {
        selectedMap: null,
        firstPick: undefined,
        allies: Array(Math.ceil(5)).fill(undefined),
        ennemies: Array(Math.ceil(5)).fill(undefined),
        allyBans: Array(Math.ceil(3)).fill(undefined),
        ennemyBans: Array(Math.ceil(3)).fill(undefined),
        selectedSlot: GenericSlotId.Map
      }
    }

    const data = reactive<Data>(getResetData())

    function getSelectedHeroRow(slot: SlotId | null) {
      if (!slot || typeof slot === 'string')
        return undefined

      switch (slot.kind)
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
    }

    const selectedHeroRow = computed(() => getSelectedHeroRow(data.selectedSlot))

    const selectedHero = computed({
      get(): Hero | undefined {
        if (!selectedHeroRow.value || !isHeroSlot(data.selectedSlot))
          return undefined
        return selectedHeroRow.value[data.selectedSlot.index]
      },
      set(value: Hero |  undefined) {
        if (!selectedHeroRow.value || !isHeroSlot(data.selectedSlot))
          return

        selectedHeroRow.value[data.selectedSlot.index] = value
      }
    })

    const order = computed(() => createSlots(data.firstPick))

    const unavailableHeroes = computed(() => data.allies.concat(data.ennemies, data.allyBans, data.ennemyBans))
    const availableHeroes = computed(() => heroes.filter(
      hero => !unavailableHeroes.value.find(availableHero => availableHero && availableHero.id === hero.id)))

    const nextSlot = computed(() => order.value.find(slot => {
      if (slot === 'map')
        return !data.selectedMap
      if (slot === 'pick')
        return !data.firstPick

      const row = getSelectedHeroRow(slot) || []
      return !row[slot.index]
    }) || null);

    const selectNextSlot = () => {
      data.selectedSlot = nextSlot.value
    }

    watch(() => data.selectedMap, selectNextSlot, { deep: true})
    watch(() => data.firstPick, selectNextSlot, { deep: true})
    watch(() => data.allies, selectNextSlot, { deep: true})
    watch(() => data.ennemies, selectNextSlot, { deep: true})
    watch(() => data.allyBans, selectNextSlot, { deep: true})
    watch(() => data.ennemyBans, selectNextSlot, { deep: true})

    return {
      ...toRefs(data),
      selectedHeroRow,
      selectedHero,
      unavailableHeroes,
      availableHeroes,
      order,
      nextSlot,

      selectedAllySlot: computed(() => getIndexFor(data.selectedSlot, 'ally')),
      selectedEnnemySlot: computed(() => getIndexFor(data.selectedSlot, 'ennemy')),
      selectedAllyBanSlot: computed(() => getIndexFor(data.selectedSlot, 'allyBan')),
      selectedEnnemyBanSlot: computed(() => getIndexFor(data.selectedSlot, 'ennemyBan')),      
      
      unsetFirstPick: () => data.firstPick = undefined,
      unselectMap: () => data.selectedMap = null,
      selectSlot: (slotId: SlotId) => data.selectedSlot = slotId,
      onAllySlotClicked: ( index: number ) => data.selectedSlot = { kind: 'ally', index },
      onEnnemySlotClicked: ( index: number ) => data.selectedSlot = { kind: 'ennemy', index },
      onAllyBanSlotClicked: ( index: number ) => data.selectedSlot = { kind: 'allyBan', index },
      onEnnemyBanSlotClicked: ( index: number ) => data.selectedSlot = { kind: 'ennemyBan', index },
      reset: () => Object.assign(data, getResetData()),
      selectNextSlot,
    }
  }
})
</script>

<style lang="scss" scoped>
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

.reset-button {
  margin: 0.2rem;
}

.map-area {
  display: flex;
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