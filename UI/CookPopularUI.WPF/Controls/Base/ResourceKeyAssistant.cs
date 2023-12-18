/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ResourceKeyAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-09-28 16:25:53
 */

using System.Windows;

namespace CookPopularUI.WPF.Controls
{
    public static class ResourceKeyAssistant
    {
        public static ResourceKey SuggestionMenuItemStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeyAssistant), ResourceKeyId.SpellingSuggestionMenuItemStyle);
        public static ResourceKey IgnoreAllMenuItemStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeyAssistant), ResourceKeyId.SpellingIgnoreAllMenuItemStyle);
        public static ResourceKey NoSuggestionsMenuItemStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeyAssistant), ResourceKeyId.SpellingNoSuggestionsMenuItemStyle);
        public static ResourceKey SeparatorStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeyAssistant), ResourceKeyId.SpellingSeparatorStyle);
        public static ResourceKey MenuItemStyleKey { get; } = new ComponentResourceKey(typeof(ResourceKeyAssistant), ResourceKeyId.MenuItemStyle);
    }

    internal enum ResourceKeyId
    {
        SpellingSuggestionMenuItemStyle,
        SpellingIgnoreAllMenuItemStyle,
        SpellingNoSuggestionsMenuItemStyle,
        SpellingSeparatorStyle,
        MenuItemStyle
    }
}
