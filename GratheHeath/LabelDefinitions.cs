﻿namespace GratheHeath
{
    public enum Eタスク一覧見出し : int
    {
        タスク大分類コード = 0,
        タスク大分類識別子 = 1,
        タスク大分類 = 2,
        タスク中分類コード = 3,
        タスク中分類識別子 = 4,
        タスク中分類 = 5,
        タスク小分類コード = 6,
        タスク小分類識別子 = 7,
        タスク小分類 = 8,
        評価項目コード = 9,
        評価項目識別子 = 10,
        評価項目 = 11
    }
    public enum EタスクXスキル対応表見出し : int
    {
        タスク小分類コード = 0,
        スキル項目コード = 1
    }
    public enum Eスキル一覧_スキル項目見出し : int
    {
        スキルカテゴリ = 0,
        スキルカテゴリ識別子 = 1,
        スキル分類コード = 2,
        スキル分類識別子 = 3,
        スキル分類 = 4,
        スキル項目コード = 5,
        スキル項目識別子 = 6,
        スキル項目 = 7
    }

    public enum Eスキル一覧_知識項目見出し : int
    {
        スキル項目コード = 0,
        スキル項目識別子 = 1,
        知識項目コード = 2,
        知識項目識別子 = 3,
        知識項目 = 4
    }

    public enum Eタスクプロフィール一覧見出し : int
    {
        タスクプロフィールコード = 0,
        タスクプロフィール種別 = 1,
        タスクプロフィール種別の説明 = 2,
        タスクプロフィールグループ = 3,
        タスクプロフィール = 4,
        タスクプロフィールの説明 = 5
    }

    public enum EタスクプロフィールＸタスク対応表見出し : int
    {
        タスクプロフィールコード = 0,
        タスク小分類コード = 1,
        対応結果 = 2
    }
}
