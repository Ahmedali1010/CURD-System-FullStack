import React, { createContext, useContext, useState, useEffect, useCallback } from 'react';
import en from '../locales/en';
import type { TranslationKeys } from '../locales/en';
import ckb from '../locales/ckb';

type Language = 'en' | 'ckb';
type Translations = typeof en;

interface I18nContextValue {
  language: Language;
  setLanguage: (lang: Language) => void;
  t: (key: TranslationKeys) => string;
  isRtl: boolean;
}

const translations: Record<Language, Translations> = { en, ckb };
const I18nContext = createContext<I18nContextValue | null>(null);

export function I18nProvider({ children }: { children: React.ReactNode }) {
  const [language, setLanguageState] = useState<Language>(
    () => (localStorage.getItem('lang') as Language) ?? 'en'
  );
  const isRtl = language === 'ckb';

  const setLanguage = useCallback((lang: Language) => {
    setLanguageState(lang);
    localStorage.setItem('lang', lang);
  }, []);

  useEffect(() => {
    document.documentElement.dir  = isRtl ? 'rtl' : 'ltr';
    document.documentElement.lang = language === 'ckb' ? 'ckb' : 'en';
  }, [language, isRtl]);

  const t = useCallback(
    (key: TranslationKeys): string =>
      translations[language][key] ?? translations.en[key] ?? String(key),
    [language]
  );

  return (
    <I18nContext.Provider value={{ language, setLanguage, t, isRtl }}>
      {children}
    </I18nContext.Provider>
  );
}

export function useI18n(): I18nContextValue {
  const ctx = useContext(I18nContext);
  if (!ctx) throw new Error('useI18n must be inside <I18nProvider>');
  return ctx;
}
