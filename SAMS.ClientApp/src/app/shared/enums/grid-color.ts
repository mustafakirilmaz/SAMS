export enum GridColor {
  Blue = 1,
  Red = 2,
  Orange = 3,
  Green = 4,
  Purple = 5,
  Yellow = 6
}

export let gridColorDescriptions: Record<keyof typeof GridColor, string> = {
  Blue: 'bg-grid bg-grid-blue',
  Red: 'bg-grid bg-grid-red',
  Orange: 'bg-grid bg-grid-orange',
  Green: 'bg-grid bg-grid-green',
  Purple: 'bg-grid bg-grid-purple',
  Yellow: 'bg-grid bg-grid-yellow'
};
