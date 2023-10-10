export enum YesNo {
  Yes = 1,
  No = 2
}

export let YesNoDescriptions: Record<keyof typeof YesNo, string> = {
  Yes: 'Evet',
  No: 'Hayır'
};
