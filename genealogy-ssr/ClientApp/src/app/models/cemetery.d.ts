import { County } from "@models";

export interface Cemetery {
  id: string;
  name: string;
  county: County;
  isRemoved: boolean;
}
