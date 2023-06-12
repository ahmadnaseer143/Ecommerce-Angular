export interface Category {
    id: number;
    category: string;
    subCategory: string;
}

export interface SuggestedProduct {
    bannerImage: string;
    category: Category;
}

export interface NavigationItem {
    category: string;
    subCategories: string[];
}